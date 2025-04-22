import React, { useState } from 'react';

interface LoginProps {
    onLoginSuccess: () => void;
}

const Login: React.FC<LoginProps> = ({ onLoginSuccess }) => {
    const [phoneNumber, setPhoneNumber] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    const handleLogin = async (e: React.FormEvent): Promise<void> => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        const credentials = { phoneNumber, password };

        try {
            const response = await fetch('https://localhost:7137/api/Auth/login', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(credentials),
            });

            if (!response.ok) {
                throw new Error('Giriş yapılamadı');
            }

            const data = await response.json();
            localStorage.setItem('authToken', data.token);
            onLoginSuccess();
        } catch (error: any) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <h2>Giriş Yap</h2>
            <form onSubmit={handleLogin}>
                <input
                    placeholder='Telefon Numarası'
                    type="text"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                    required
                />
                <br />
                
                <input
                    placeholder='Şifre'
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit" disabled={loading}>Giriş Yap</button>
            </form>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {loading && <p>Yükleniyor...</p>}
        </div>
    );
};

export default Login;
