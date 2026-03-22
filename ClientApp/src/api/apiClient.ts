import axios from 'axios';

const configuredBase = import.meta.env.VITE_API_BASE_URL as string | undefined;
const defaultBase = 'http://localhost:5293/api';

// Create an Axios instance with base configuration
const apiClient = axios.create({
    baseURL: configuredBase || defaultBase,
    headers: {
        'Content-Type': 'application/json',
    },
});

// Add a request interceptor to include the JWT token in all authenticated requests
apiClient.interceptors.request.use(
    (config) => {
        const token = localStorage.getItem('token');
        if (token) {
            config.headers['Authorization'] = `Bearer ${token}`;
        }
        return config;
    },
    (error) => {
        return Promise.reject(error);
    }
);

// Add a response interceptor to handle common errors like 401 Unauthorized
apiClient.interceptors.response.use(
    (response) => {
        return response;
    },
    (error) => {
        if (error.response && error.response.status === 401) {
            // Handle unauthorized access (e.g., clear token, redirect to login)
            console.error('Unauthorized access. Token might be expired.');
            localStorage.removeItem('token');
            localStorage.removeItem('user');
            // We shouldn't use window.location here directly in React Router typically, 
            // but for a hard redirect on 401, it's sometimes necessary if not using a global auth provider Context.
        }
        return Promise.reject(error);
    }
);

export default apiClient;
