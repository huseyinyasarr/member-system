import React, { useState } from 'react';

interface SignUpProps {
    onUserAdded: () => void;  // Kullanıcı eklendikten sonra kullanıcı listesini güncellemek için
}

const SignUp: React.FC<SignUpProps> = ({ onUserAdded }) => {
    const [firstName, setFirstName] = useState('');
    const [lastName, setLastName] = useState('');
    const [phoneNumber, setPhoneNumber] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState<string | null>(null);  // Hata durumu
    const [loading, setLoading] = useState(false);  // Yükleniyor durumu

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setLoading(true);
        setError(null);  // Hata mesajını sıfırla

        const newUser = {
            firstName,
            lastName,
            phoneNumber,
            password
        };

        try {
            const response = await fetch('https://localhost:7137/api/users', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(newUser)
            });

            if (!response.ok) {
                throw new Error('Kullanıcı eklenemedi');
            }

            await response.json();
            onUserAdded();  // Yeni kullanıcı eklendikten sonra listeyi güncelle
            setFirstName('');
            setLastName('');
            setPhoneNumber('');
            setPassword('');
        } catch (error: any) {
            setError(error.message);  // Hata oluşursa hata mesajını state'e kaydet
        } finally {
            setLoading(false);  // Yükleniyor durumunu kapat
        }
    };

    return (
        <div>
            <h2>Sign Up</h2>
            <form onSubmit={handleSubmit}>
                <label>Ad:</label>
                <input
                    type="text"
                    value={firstName}
                    onChange={(e) => setFirstName(e.target.value)}
                    required
                />
                <label>Soyad:</label>
                <input
                    type="text"
                    value={lastName}
                    onChange={(e) => setLastName(e.target.value)}
                    required
                />
                <label>Telefon Numarası:</label>
                <input
                    type="text"
                    value={phoneNumber}
                    onChange={(e) => setPhoneNumber(e.target.value)}
                    required
                />
                <label>Şifre:</label>
                <input
                    type="password"
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                    required
                />
                <button type="submit" disabled={loading}>Kayıt Ol</button>
            </form>
            {error && <p style={{ color: 'red' }}>{error}</p>}
            {loading && <p>Yükleniyor...</p>}
        </div>
    );
};

export default SignUp;
