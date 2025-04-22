import React, { useState, useEffect } from 'react';
import Login from './components/Login';
import SignUp from './components/SignUp';
import UserList from './components/UserList';

const App: React.FC = () => {
    const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
    const [isSignUp, setIsSignUp] = useState<boolean>(false);

    // Sayfa yüklendiğinde token kontrolü yapılacak
    useEffect(() => {
        const token = localStorage.getItem('authToken');
        if (token) {
            setIsAuthenticated(true);
        } else {
            setIsAuthenticated(false);
        }
    }, []);

    // Giriş başarılı olunca yönlendirme yapılacak
    const handleLoginSuccess = (): void => {
        setIsAuthenticated(true);
    };

    // Kayıt başarılı olunca yönlendirme yapılacak
    const handleSignUpSuccess = (): void => {
        setIsSignUp(false); // SignUp sayfası kapatılıyor
        setIsAuthenticated(true);
    };

    // Çıkış işlemi
    const handleLogout = () => {
        localStorage.removeItem('authToken');
        setIsAuthenticated(false); // Çıkış yapınca kullanıcı login sayfasına yönlendirilecek
    };

    return (
        <div>
            <h1>Member System</h1>
            {/* Eğer kullanıcı giriş yapmadıysa login ya da signup sayfası render edilecek */}
            {!isAuthenticated && !isSignUp && (
                <div>
                    <button onClick={() => setIsSignUp(true)}>Kayıt Ol</button>
                    <Login onLoginSuccess={handleLoginSuccess} />
                </div>
            )}

            {/* Kayıt olmak için SignUp sayfası */}
            {isSignUp && !isAuthenticated && (
                <SignUp onSignUpSuccess={handleSignUpSuccess} />
            )}

            {/* Kullanıcı login olduktan sonra kullanıcı listesi gösterilecek */}
            {isAuthenticated && (
                <UserList handleLogout={handleLogout} />
            )}
        </div>
    );
};

export default App;



// import React, { useState } from 'react';
// import Login from './components/Login';
// import SignUp from './components/SignUp';
// import UserList from './components/UserList';
// import history from './services/useHistory'; // Yönlendirme servisini dahil ediyoruz

// const App: React.FC = () => {
//     const [isAuthenticated, setIsAuthenticated] = useState<boolean>(false);
//     const [isSignUp, setIsSignUp] = useState<boolean>(false);

//     const handleLoginSuccess = (): void => {
//         setIsAuthenticated(true);
//         history.push("/users");  // Giriş başarılı olduğunda kullanıcıyı kullanıcı listesine yönlendir
//     };

//     const handleSignUpSuccess = (): void => {
//         setIsSignUp(false);  // Kayıt işlemi başarılı olursa SignUp formunu kapat
//         setIsAuthenticated(true);
//         history.push("/");  // Kayıt başarılı olduğunda kullanıcıyı ana sayfaya yönlendir
//     };

//     return (
//         <div>
//             <h1>Member System</h1>
//             {history.getCurrentPath() === "/" && !isAuthenticated && !isSignUp && (
//                 <div>
//                     <button onClick={() => setIsSignUp(true)}>Kayıt Ol</button>
//                     <Login onLoginSuccess={handleLoginSuccess} />
//                 </div>
//             )}
//             {history.getCurrentPath() === "/" && !isAuthenticated && isSignUp && (
//                 <SignUp onSignUpSuccess={handleSignUpSuccess} />
//             )}
//             {history.getCurrentPath() === "/users" && isAuthenticated && <UserList />}
//         </div>
//     );
// };

// export default App;
