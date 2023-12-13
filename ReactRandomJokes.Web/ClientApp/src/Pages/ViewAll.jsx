import React, { useState, useEffect } from 'React';
import axios from 'axios';
import ViewAllJoke from '../Components/ViewAllJoke';

const ViewAll = () => {
    const [jokes, setJokes] = useState([]);

    useEffect(() => {
        const loadJokes = async () => {
            const { data } = await axios.get('/api/jokes/getall');
            setJokes(data);
        }
        loadJokes();
    }, [])

    return (
        <div>
            {jokes.map(joke => <ViewAllJoke key={joke.id} joke={joke }/>)}
        </div>
        )
}

export default ViewAll;