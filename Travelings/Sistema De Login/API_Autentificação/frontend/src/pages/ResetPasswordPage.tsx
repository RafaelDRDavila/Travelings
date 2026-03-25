import { useMemo, useState } from 'react'
import { Link, useNavigate, useSearchParams } from 'react-router-dom'
import { api } from '../api/client'

export default function ResetPasswordPage() {
  const [searchParams] = useSearchParams()
  const navigate = useNavigate()

  const email = useMemo(() => searchParams.get('email') ?? '', [searchParams])
  const token = useMemo(() => {
    const raw = searchParams.get('token') ?? ''
    try {
      return decodeURIComponent(raw)
    } catch {
      return raw
    }
  }, [searchParams])

  const [newPassword, setNewPassword] = useState('')
  const [confirmNewPassword, setConfirmNewPassword] = useState('')
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')

  const canSubmit = email && token

  async function handleSubmit(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    if (!canSubmit) {
      setError('Link inválido. Solicite um novo link de redefinição.')
      return
    }
    if (newPassword !== confirmNewPassword) {
      setError('As senhas não conferem.')
      return
    }

    setLoading(true)
    try {
      await api.post('/auth/reset-password', {
        email,
        token,
        newPassword,
        confirmNewPassword,
      })
      navigate('/login', { replace: true })
    } catch (err: unknown) {
      const message =
        err && typeof err === 'object' && 'response' in err
          ? (err as { response?: { data?: { message?: string } } }).response?.data?.message
          : 'Não foi possível redefinir a senha.'
      setError(message ?? 'Não foi possível redefinir a senha.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="auth-shell">
      <div className="card">
        <h1>Redefinir senha</h1>
        <p className="sub">Crie uma nova senha para sua conta</p>

        <form className="form" onSubmit={handleSubmit}>
          {error && <p className="error">{error}</p>}

          {!canSubmit && (
            <p style={{ color: 'var(--muted)', fontSize: 14 }}>
              Link inválido. Volte e solicite novamente a redefinição.
            </p>
          )}

          <div className="field">
            <label htmlFor="newPassword">Nova senha</label>
            <input
              id="newPassword"
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
              minLength={6}
              autoComplete="new-password"
              placeholder="Mínimo 6 caracteres"
            />
          </div>

          <div className="field">
            <label htmlFor="confirmNewPassword">Confirmar nova senha</label>
            <input
              id="confirmNewPassword"
              type="password"
              value={confirmNewPassword}
              onChange={(e) => setConfirmNewPassword(e.target.value)}
              required
              minLength={6}
              autoComplete="new-password"
              placeholder="Repita a senha"
            />
          </div>

          <button className="button" type="submit" disabled={loading || !canSubmit}>
            {loading ? 'Salvando...' : 'Redefinir senha'}
          </button>
        </form>

        <div className="footer">
          <Link to="/forgot-password">Solicitar novo link</Link>
          <span style={{ margin: '0 8px' }}>·</span>
          <Link to="/login">Login</Link>
        </div>
      </div>
    </div>
  )
}

