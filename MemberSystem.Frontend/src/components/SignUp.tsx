import React, { useState } from 'react';
import history from '../services/useHistory'; // Yönlendirme servisi

interface SignUpProps {
    onSignUpSuccess: () => void;
}

const SignUp: React.FC<SignUpProps> = ({ onSignUpSuccess }) => {
    const [firstName, setFirstName] = useState<string>('');
    const [lastName, setLastName] = useState<string>('');
    const [phoneNumber, setPhoneNumber] = useState<string>('');
    const [password, setPassword] = useState<string>('');
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    const handleSignUp = async (e: React.FormEvent): Promise<void> => {
        e.preventDefault();
        setLoading(true);
        setError(null);

        const newUser = { firstName, lastName, phoneNumber, password };

        try {
            const response = await fetch('https://localhost:7137/api/Users', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newUser),
            });

            if (!response.ok) {
                throw new Error('Kayıt işlemi başarısız');
            }

            await response.json();
            // Kayıt başarılı olunca ana sayfaya yönlendir
            history.push('/');  // Ana sayfaya yönlendirme yapıyoruz
            onSignUpSuccess();  // Kayıt işlemi başarılı olduğunda çağrılır

        } catch (error: any) {
            setError(error.message);
        } finally {
            setLoading(false);
        }
    };

    return (
        <div>
            <form onSubmit={handleSignUp}>
                <input
                    placeholder='Ad'
                    type="text"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                    required
                />
                <br />

                <input
                    placeholder='Soyad'
                    type="text"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                    required
                />
                <br />

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
                <button type="submit" disabled={loading}>Kayıt Ol</button>
            </form>
            {loading && <p>Yükleniyor...</p>}
        </div>
    );
};

export default SignUp;
