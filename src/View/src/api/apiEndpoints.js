const API_BASE_URL = import.meta.env.VITE_API_BASE_URL;

export const userEndpoints = {
    default: `${API_BASE_URL}/users`,
    sign_in: `${API_BASE_URL}/users/sign-in`,
    sign_up: `${API_BASE_URL}/users/sign-up`
};

export const fileEndpoints = {
    default: `${API_BASE_URL}/files`,
    encode: `${API_BASE_URL}/files/encode`,
    decode: `${API_BASE_URL}/files/decode`
}
