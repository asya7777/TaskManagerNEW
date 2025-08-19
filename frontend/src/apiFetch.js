//any request that hits an endpoint protected with [Authorize] must include the JWT token. bununiçin helper

export async function apiFetch(url, options = {})//the api endpoint i want to call & an object where you can pass things to
{
    const token = localStorage.getItem('token');

    const headers = {
        'Content-Type': 'application/json',
        ...(options.headers || {})//there might be headers here, or nothing
    }
    
    if (token) {//if a token exists, add an authorization header
        headers['Authorization'] = `Bearer ${token}`;
    }

    return fetch(url, { ...options, headers });//"spreads" options, adding header
}