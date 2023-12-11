import React, { useState } from 'react'
import { useNavigate } from 'react-router-dom';
import axios from 'axios';

const SignUp = () => {

    const [formData, setFormData] = useState({
        firstName: '',
        lastName: '',
        email: '',
        password:''
    });

    const navigate = useNavigate();

    const onTextChange = e => {
        const copy = { ...formData };
        copy[e.target.name] = e.target.value;
        setFormData(copy);
    }

    const onFormSubmit = async e => {
        e.preventDefault();
        await axios.post('/api/account/signup', formData);
        navigate('/login');
    }

    return(
    <div className='container pt-5'>
        <div className="row" style={{ display: 'flex', alignItems: 'center' }}>
            <div className="col-md-6 offset-md-3 bg light p-4 rounded shadow">
                    <h3>Sign up for a new account</h3>
                    <form onSubmit={onFormSubmit}>
                        <input type="text" name="firstName" placeholder="First Name" className="form-control" value={formData.firstName} onChange={onTextChange} />
                        <input type="text" name="lastName" placeholder="Last Name" className="form-control mt-3" value={formData.lastName} onChange={onTextChange} />
                        <input type="text" name="email" placeholder="Email" className="form-control mt-3" value={formData.email} onChange={onTextChange} />
                        <input type="password" name="password" placeholder="Password" className="form-control mt-3" value={formData.password} onChange={onTextChange} />
                    <button className="btn btn-primary mt-3">Sign Up</button>
                </form>
            </div>
        </div>
        </div>
        )
}
export default SignUp;