<template>
    <div class="homepage">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
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

        <main class="d-flex flex-column align-items-center justify-content-center vh-100 text-center">
            <h1 class="display-3 fw-bold mb-4">Welcome to Task Manager</h1>
            <router-link v-if="isLoggedIn" to="/user-page">Start managing your tasks</router-link>
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
    .custom-btn.btn:hover {
        background-color: darkmagenta;
    }
</style>