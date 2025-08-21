import { ref } from "vue";

export const isLoggedIn = ref(!!localStorage.getItem("token"));//bunu logged in mi diye bakmak i�in kullan�caz. !! boolean conversion


export const logout = () => {
    localStorage.removeItem('userId');
    localStorage.removeItem('token');
    localStorage.removeItem('firstName');
    localStorage.removeItem('userRole');


    isLoggedIn.value = false;
};