
import React, { useState, useEffect } from 'react';

const UserList: React.FC<{ handleLogout: () => void }> = ({ handleLogout }) => {
    const [users, setUsers] = useState<any[]>([]);
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        const fetchUsers = async () => {
            setLoading(true);
            setError(null);

            const token = localStorage.getItem('authToken'); // JWT token'ı localStorage'dan al

            if (!token) {
                // Eğer token yoksa giriş sayfasına yönlendir
                handleLogout();
                return;
            }

            try {
                const response = await fetch('https://localhost:7137/api/Users', {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json',
                        'Authorization': `Bearer ${token}`,  // JWT token'ı header'a ekle
                    },
                });

                if (!response.ok) {
                    throw new Error('Kullanıcılar alınamadı');
                }

                const data = await response.json();
                setUsers(data); // Kullanıcıları state'e kaydet
            } catch (error: any) {
                setError(error.message);
            } finally {
                setLoading(false);
            }
        };

        fetchUsers();
    }, [handleLogout]); // handleLogout fonksiyonu da dependency olarak ekledik

    return (
        <div>
            <h2>Kullanıcı Listesi</h2>
            <button 
                onClick={handleLogout} 
                style={{
                    marginBottom: '10px',
                    padding: '8px 16px',
                    backgroundColor: '#ff4d4d',
                    color: 'white',
                    border: 'none',
                    cursor: 'pointer'
                }}
            >
                Çıkış Yap
            </button>
            {loading && <p>Yükleniyor...</p>}
            {error && <p style={{ color: 'red' }}>{error}</p>}
            
            {!loading && !error && (
                <table style={{ width: '50%', margin: '0 auto', borderCollapse: 'collapse' }}>
                    <thead>
                        <tr>
                            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Adı</th>
                            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Soyadı</th>
                            <th style={{ border: '1px solid #ddd', padding: '8px' }}>Telefon Numarası</th>
                        </tr>
                    </thead>
                    <tbody>
                        {users.map((user) => (
                            <tr key={user.id}>
                                <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.firstName}</td>
                                <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.lastName}</td>
                                <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.phoneNumber}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default UserList;

// import React, { useState, useEffect } from 'react';
// import history from '../services/useHistory'; // Yönlendirme servisi

// const UserList: React.FC = () => {
//     const [users, setUsers] = useState<any[]>([]);
//     const [error, setError] = useState<string | null>(null);
//     const [loading, setLoading] = useState<boolean>(false);

//     useEffect(() => {
//         const fetchUsers = async () => {
//             setLoading(true);
//             setError(null);

//             const token = localStorage.getItem('authToken');  // JWT token'ı localStorage'dan al

//             if (!token) {
//                 // Eğer token yoksa giriş sayfasına yönlendir
//                 history.push('/');
//                 return;
//             }

//             try {
//                 const response = await fetch('https://localhost:7137/api/Users', {
//                     method: 'GET',
//                     headers: {
//                         'Content-Type': 'application/json',
//                         'Authorization': `Bearer ${token}`,  // JWT token'ı header'a ekle
//                     },
//                 });

//                 if (!response.ok) {
//                     throw new Error('Kullanıcılar alınamadı');
//                 }

//                 const data = await response.json();
//                 setUsers(data);  // Kullanıcıları state'e kaydet
//             } catch (error: any) {
//                 setError(error.message);
//             } finally {
//                 setLoading(false);
//             }
//         };

//         fetchUsers();
//     }, []);

//     // Çıkış yapma işlemi
//     const handleLogout = () => {
//         localStorage.removeItem('authToken');  // Token'ı kaldır
//         history.push('/');  // Ana sayfaya yönlendir
//     };

//     return (
//         <div>
//             <h2>Kullanıcı Listesi</h2>
//             <button 
//                 onClick={handleLogout} 
//                 style={{
//                     marginBottom: '10px',
//                     padding: '8px 16px',
//                     backgroundColor: '#ff4d4d',
//                     color: 'white',
//                     border: 'none',
//                     cursor: 'pointer'
//                 }}
//             >
//                 Çıkış Yap
//             </button>
//             {loading && <p>Yükleniyor...</p>}
//             {error && <p style={{ color: 'red' }}>{error}</p>}
            
//             {!loading && !error && (
//                 <table style={{ width: '50%', margin: '0 auto', borderCollapse: 'collapse' }}>
//                     <thead>
//                         <tr>
//                             <th style={{ border: '1px solid #ddd', padding: '8px' }}>Adı</th>
//                             <th style={{ border: '1px solid #ddd', padding: '8px' }}>Soyadı</th>
//                             <th style={{ border: '1px solid #ddd', padding: '8px' }}>Telefon Numarası</th>
//                         </tr>
//                     </thead>
//                     <tbody>
//                         {users.map((user) => (
//                             <tr key={user.id}>
//                                 <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.firstName}</td>
//                                 <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.lastName}</td>
//                                 <td style={{ border: '1px solid #ddd', padding: '8px' }}>{user.phoneNumber}</td>
//                             </tr>
//                         ))}
//                     </tbody>
//                 </table>
//             )}
//         </div>
//     );
// };

// export default UserList;
