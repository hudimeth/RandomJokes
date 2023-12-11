import React, { useState } from 'react'
import axios from 'axios';
import { useNavigate, Link } from 'react-router-dom';
import { useAuth } from '../Components/AuthContextComponent';

const LogIn = () => {

    const { setUser } = useAuth();
    const [formData, setFormData] = useState({
        email: '',
        password: ''
    });
    const [isValidLogin, setIsValidLogin] = useState(true);

    const navigate = useNavigate();

    const onTextChange = e => {
        const copy = { ...formData };
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        const { data } = await axios.post('/api/account/login', formData);
        const isValid = !!data;
        setIsValidLogin(isValid);
        if (isValid) {
            setUser(data);
            console.log(data);
            navigate('/');
        }
    }

    return (
        <div className='container pt-5'>
            <div className="row" style={{ display: 'flex', alignItems: 'center' }}>
                <div className="col-md-6 offset-md-3 bg light p-4 rounded shadow">
                    <h3>Log in to your account</h3>
                    {!isValidLogin && <span className='text-danger'>Invalid username/password. Please try again.</span>}
                    <form onSubmit={onFormSubmit}>
                        <input type="text" name="email" placeholder="Email" className="form-control mt-3" value={formData.email} onChange={onTextChange} />
                        <input type="password" name="password" placeholder="Password" className="form-control mt-3" value={formData.password} onChange={onTextChange} />
                        <button className="btn btn-primary mt-3">Log In</button>
                    </form>
                    <div className='offset-md-3'>
                        <Link to='/signup'>Don't have an account with us? Sign up here.</Link>
                    </div>
                </div>
            </div>
        </div>
    )
}
export default LogIn;