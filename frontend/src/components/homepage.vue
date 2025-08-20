<template>
    <div class="homepage">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="#">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50"  />
                </a>

                <div class="collapse navbar-collapse">
                    <ul class="navbar-nav ms-auto gap-4">
                        <li class="nav-item">
                            <router-link class="btn  btn-lg" to="/login">Login</router-link>
                        </li>
                        <li class="nav-item">
                            <router-link class="btn  btn-lg" to="/register">Sign up</router-link>
                        </li>
                    </ul>
                </div>
            </div>
        </header>

        <main class="d-flex flex-column align-items-center justify-content-center vh-100 text-center">
            <h1 class="display-3 fw-bold mb-4">Welcome to Task Manager</h1>
        </main>
    </div>
</template>


<script>
</script>

<style>
    .btn:hover{
        background-color:mediumseagreen;
    }
</style>