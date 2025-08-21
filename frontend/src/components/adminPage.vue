<template>
    <div class="homepage d-flex flex-column vh-100">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="/">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50" />
                </a>
                <button class="btn btn-outline-danger" @click="handleLogout">Logout</button>
            </div>
        </header>

        <div class="d-flex">

            <h2 class="font-class p-10 fs-1 ">Welcome, {{firstName}}!</h2>


        </div>

    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import CreateTask from './CreateTask.vue';
    import { apiFetch } from '../apiFetch';
    import { logout } from "../logout";
    import { useRouter } from 'vue-router';

    const router = useRouter();

    onMounted(async () => {
        firstName.value = localStorage.getItem('firstName');

        try {
            const response = await apiFetch('http://localhost:5022/api/Task/my-tasks');
            assignedTasks.value = await response.json();//pause until backend responds
        }catch (err) {
            console.error('Error fetching tasks:', err);
        }
    });

    const handleLogout = () => {
        logout();
        router.push('/');
    }
</script>

<style scoped>
    .sidebar {
        width: 300px;
    }

    .soft-yellow {
        background-color: #FFF9DB; /* Soft pastel yellow */
    }

    .font-class {
        font-family: 'Book Antiqua';
    }
</style>