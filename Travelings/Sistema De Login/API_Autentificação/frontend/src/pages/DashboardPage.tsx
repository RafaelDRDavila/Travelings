import { useAuth } from '../contexts/AuthContext';
import { Link } from 'react-router-dom';

export default function DashboardPage() {
  const { user, logout } = useAuth();

  return (
    <div className="auth-shell">
      <div className="card">
        <h1>Dashboard</h1>
        <p className="sub">Sessão autenticada</p>

        {user && (
          <div className="form">
            <div>
              <p>
                Olá, <strong>{user.email}</strong>
              </p>
              <p style={{ color: 'var(--muted)', fontSize: 13, marginTop: 6 }}>
                ID: {user.id}
              </p>
            </div>
          </div>
        )}

        <button className="button secondary" type="button" onClick={logout}>
          Sair
        </button>

        <div className="footer">
          <Link to="/">Voltar</Link>
        </div>
      </div>
    </div>
  );
}
