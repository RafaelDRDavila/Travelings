import { useState } from 'react'
import { Link } from 'react-router-dom'
import { useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext'

export default function ForgotPasswordPage() {
  const navigate = useNavigate()
  const { requestRecoveryCode, verifyRecoveryCode, completeRecovery } = useAuth()

  const [email, setEmail] = useState('')
  const [code, setCode] = useState('')
  const [ticket, setTicket] = useState<string | null>(null)
  const [step, setStep] = useState<'request' | 'verify' | 'choice' | 'change'>('request')
  const [loading, setLoading] = useState(false)
  const [done, setDone] = useState(false)
  const [error, setError] = useState('')
  const [newPassword, setNewPassword] = useState('')
  const [confirmNewPassword, setConfirmNewPassword] = useState('')

  async function handleRequestCode(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      await requestRecoveryCode(email)
      setDone(true)
      setStep('verify')
    } catch (err: unknown) {
      const message =
        err && typeof err === 'object' && 'response' in err
          ? (err as { response?: { data?: { message?: string } } }).response?.data?.message
          : 'Não foi possível enviar a solicitação.'
      setError(message ?? 'Não foi possível enviar a solicitação.')
    } finally {
      setLoading(false)
    }
  }

  async function handleVerifyCode(e: React.FormEvent) {
    e.preventDefault()
    setError('')
    setLoading(true)
    try {
      const t = await verifyRecoveryCode(email, code)
      setTicket(t)
      setStep('choice')
    } catch (err: unknown) {
      const message =
        err && typeof err === 'object' && 'response' in err
          ? (err as { response?: { data?: { message?: string } } }).response?.data?.message
          : 'Código inválido ou expirado.'
      setError(message ?? 'Código inválido ou expirado.')
    } finally {
      setLoading(false)
    }
  }

  async function handleAccess() {
    if (!ticket) return
    setError('')
    setLoading(true)
    try {
      await completeRecovery({ ticket, action: 'access' })
      navigate('/dashboard', { replace: true })
    } catch (err: unknown) {
      const message =
        err && typeof err === 'object' && 'response' in err
          ? (err as { response?: { data?: { message?: string } } }).response?.data?.message
          : 'Não foi possível acessar a conta.'
      setError(message ?? 'Não foi possível acessar a conta.')
    } finally {
      setLoading(false)
    }
  }

  async function handleChangePassword(e: React.FormEvent) {
    e.preventDefault()
    if (!ticket) return
    setError('')
    if (newPassword !== confirmNewPassword) {
      setError('As senhas não conferem.')
      return
    }
    setLoading(true)
    try {
      await completeRecovery({
        ticket,
        action: 'change_password',
        newPassword,
        confirmNewPassword,
      })
      navigate('/dashboard', { replace: true })
    } catch (err: unknown) {
      const message =
        err && typeof err === 'object' && 'response' in err
          ? (err as { response?: { data?: { message?: string } } }).response?.data?.message
          : 'Não foi possível alterar a senha.'
      setError(message ?? 'Não foi possível alterar a senha.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <div className="auth-shell">
      <div className="card">
        <h1>Esqueci minha senha</h1>
        <p className="sub">Informe seu e-mail para receber um código de verificação</p>

        <form
          className="form"
          onSubmit={
            step === 'verify' ? handleVerifyCode : step === 'change' ? handleChangePassword : handleRequestCode
          }
        >
          {error && <p className="error">{error}</p>}

          {done && step === 'request' ? (
            <p style={{ color: 'var(--muted)', fontSize: 14 }}>
              Se o e-mail existir, enviaremos um código de verificação.
            </p>
          ) : null}

          {step === 'request' ? (
            <div className="field">
              <label htmlFor="email">E-mail</label>
              <input
                id="email"
                type="email"
                value={email}
                onChange={(e) => setEmail(e.target.value)}
                required
                autoComplete="email"
                placeholder="voce@exemplo.com"
              />
            </div>
          ) : null}

          {step === 'verify' ? (
            <>
              <div className="field">
                <label htmlFor="email">E-mail</label>
                <input id="email" type="email" value={email} disabled />
              </div>
              <div className="field">
                <label htmlFor="code">Código</label>
                <input
                  id="code"
                  inputMode="numeric"
                  value={code}
                  onChange={(e) => setCode(e.target.value)}
                  required
                  placeholder="000000"
                />
              </div>
              <button className="button" type="submit" disabled={loading}>
                {loading ? 'Verificando...' : 'Verificar código'}
              </button>
            </>
          ) : null}

          {step === 'choice' ? (
            <>
              <p style={{ color: 'var(--muted)', fontSize: 14 }}>
                Código verificado. O que você deseja fazer?
              </p>
              <button className="button" type="button" disabled={loading} onClick={handleAccess}>
                {loading ? 'Aguarde...' : 'Acessar conta'}
              </button>
              <button
                className="button"
                type="button"
                disabled={loading}
                onClick={() => setStep('change')}
                style={{ background: 'transparent', border: '1px solid var(--border)', color: 'var(--text)' }}
              >
                Alterar senha
              </button>
            </>
          ) : null}

          {step === 'change' ? (
            <>
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
              <button className="button" type="submit" disabled={loading}>
                {loading ? 'Salvando...' : 'Salvar e entrar'}
              </button>
            </>
          ) : null}

          {step === 'request' && (
            <button className="button" type="submit" disabled={loading}>
              {loading ? 'Enviando...' : 'Enviar código'}
            </button>
          )}
        </form>

        <div className="footer">
          <Link to="/login">Voltar para login</Link>
        </div>
      </div>
    </div>
  )
}

