import Cookies from 'js-cookie';

export function getJwtFromCookie() {
    const cookieValue = Cookies.get('jwt');

    return cookieValue ? cookieValue : null;
}