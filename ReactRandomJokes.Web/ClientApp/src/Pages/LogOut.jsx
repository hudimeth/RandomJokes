import React, { useEffect } from 'react';
import { useAuth } from '../Components/AuthContextComponent';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

const LogOut = () => {

    const { setUser, user } = useAuth();
    const navigate = useNavigate();

    useEffect(() => {
        const logOut = async () => {
            await axios.post('/api/account/logout');
            setUser(null);
            navigate('/');
        }
        logOut();
    }, [])
    return(<></>)
}
export default LogOut;