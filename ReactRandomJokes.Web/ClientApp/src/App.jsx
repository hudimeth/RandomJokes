import React from 'react';
import Layout from './Components/Layout';
import { Routes, Route } from 'react-router';
import Home from './Pages/Home';
import LogIn from './Pages/LogIn';
import SignUp from './Pages/SignUp';
import LogOut from './Pages/LogOut';
import { AuthContextComponent } from './Components/AuthContextComponent';

const App = () => {
    return (
        <AuthContextComponent>
            <Layout>
                <Routes>
                    <Route exact path='/' element={<Home />} />
                    <Route exact path='/signup' element={<SignUp />} />
                    <Route exact path='/login' element={<LogIn />} />
                    <Route exact path='/logout' element={<LogOut />} />
                </Routes>
            </Layout>
        </AuthContextComponent>
    )
}
export default App;