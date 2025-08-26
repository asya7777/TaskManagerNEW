<template>
    <div class="homepage">
        <header class="navbar navbar-expand-lg bg-light navbar-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="#">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50"  />
                </a>

                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav ms-auto gap-4">
                        <li v-if="!isLoggedIn" class="nav-item">
                            <router-link class="custom-btn btn  btn-lg" to="/login">Login</router-link>
                        </li>
                        <li v-if="!isLoggedIn" class="nav-item">
                            <router-link class="custom-btn btn btn-lg" to="/register">Sign up</router-link>
                        </li>
                        <li v-else>
                            <button class="custom-btn btn" @click="handleLogout">Logout</button>
                        </li>
                    </ul>
                </div>
            </div>
        </header>

        <main class="homepage-main d-flex flex-column align-items-center justify-content-center vh-100 text-center">
            <h1 class="welcome-title display-3 fw-bold mb-">Welcome to Task Manager</h1>
            <router-link class="redirect-touser " v-if="isLoggedIn" to="/user-page">Start managing your tasks</router-link>
        </main>
    </div>
</template>


<script setup>
    import { ref} from "vue";
    import { isLoggedIn, logout } from "../logout";

    const handleLogout = () => {
        logout();
    }
</script>

<style>
    .homepage-main {
        background-image: url('../assets/images/background.jpg');
        padding-bottom: 190px;  
    }
    .redirect-touser{
        font-size:1.5rem;
        color:black;
        text-decoration:underline;
    }
    .custom-btn.btn:hover {
        background-color: darkmagenta;
    }
    .welcome-title{
        color:black;
    }
</style>