import {
  createContext,
  useCallback,
  useContext,
  useEffect,
  useState,
  type ReactNode,
} from 'react';
import {
  api,
  clearStoredAuth,
  getStoredAccessToken,
  setStoredTokens,
  getStoredUser,
  type AuthResponse,
  type UserResponse,
} from '../api/client';

type User = { id: string; email: string; emailConfirmed?: boolean; createdAt?: string };

type AuthContextValue = {
  user: User | null;
  isLoading: boolean;
  isAuthenticated: boolean;
  login: (email: string, password: string) => Promise<void>;
  register: (userName: string, email: string, password: string, confirmPassword: string) => Promise<void>;
  requestRecoveryCode: (email: string) => Promise<void>;
  verifyRecoveryCode: (email: string, code: string) => Promise<string>;
  completeRecovery: (args: {
    ticket: string;
    action: 'access' | 'change_password';
    newPassword?: string;
    confirmNewPassword?: string;
  }) => Promise<void>;
  logout: () => void;
  fetchUser: () => Promise<void>;
};

const AuthContext = createContext<AuthContextValue | null>(null);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(() => getStoredUser());
  const [isLoading, setIsLoading] = useState(true);

  const fetchUser = useCallback(async () => {
    const token = getStoredAccessToken();
    if (!token) {
      setUser(null);
      setIsLoading(false);
      return;
    }
    try {
      const { data } = await api.get<UserResponse>('/auth/me');
      setUser({
        id: data.id,
        email: data.email,
        emailConfirmed: data.emailConfirmed,
        createdAt: data.createdAt,
      });
      const stored = getStoredUser();
      if (stored) {
        sessionStorage.setItem(
          'auth_user',
          JSON.stringify({
            id: data.id,
            email: data.email,
            emailConfirmed: data.emailConfirmed,
            createdAt: data.createdAt,
          })
        );
      }
    } catch {
      setUser(null);
      clearStoredAuth();
    } finally {
      setIsLoading(false);
    }
  }, []);

  useEffect(() => {
    fetchUser();
  }, [fetchUser]);

  useEffect(() => {
    const onLogout = () => {
      setUser(null);
    };
    window.addEventListener('auth-logout', onLogout);
    return () => window.removeEventListener('auth-logout', onLogout);
  }, []);

  const login = useCallback(
    async (email: string, password: string) => {
      const { data } = await api.post<AuthResponse>('/auth/login', { email, password });
      setStoredTokens(data.accessToken, data.refreshToken, { id: data.userId, email: data.email });
      setUser({ id: data.userId, email: data.email });
    },
    []
  );

  const register = useCallback(
    async (userName: string, email: string, password: string, confirmPassword: string) => {
      const { data } = await api.post<AuthResponse>('/auth/register', {
        userName,
        email,
        password,
        confirmPassword,
      });
      setStoredTokens(data.accessToken, data.refreshToken, { id: data.userId, email: data.email });
      setUser({ id: data.userId, email: data.email });
    },
    []
  );

  const requestRecoveryCode = useCallback(async (email: string) => {
    await api.post('/auth/password-recovery/request-code', { email });
  }, []);

  const verifyRecoveryCode = useCallback(async (email: string, code: string) => {
    const { data } = await api.post<{ ticket: string }>('/auth/password-recovery/verify-code', { email, code });
    return data.ticket;
  }, []);

  const completeRecovery = useCallback(
    async (args: {
      ticket: string;
      action: 'access' | 'change_password';
      newPassword?: string;
      confirmNewPassword?: string;
    }) => {
      const { data } = await api.post<AuthResponse>('/auth/password-recovery/complete', {
        ticket: args.ticket,
        action: args.action,
        newPassword: args.newPassword ?? null,
        confirmNewPassword: args.confirmNewPassword ?? null,
      });

      setStoredTokens(data.accessToken, data.refreshToken, { id: data.userId, email: data.email });
      setUser({ id: data.userId, email: data.email });
    },
    []
  );

  const logout = useCallback(() => {
    clearStoredAuth();
    setUser(null);
  }, []);

  const value: AuthContextValue = {
    user,
    isLoading,
    isAuthenticated: !!user,
    login,
    register,
    requestRecoveryCode,
    verifyRecoveryCode,
    completeRecovery,
    logout,
    fetchUser,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const ctx = useContext(AuthContext);
  if (!ctx) throw new Error('useAuth must be used within AuthProvider');
  return ctx;
}
