<template>
    <div class="admin-page d-flex flex-column vh-100">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="/">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50" />
                </a>

                <h1>ADMIN</h1>
                <button class="btn" @click="handleLogout">Logout</button>
            </div>

        </header>


        <h2 class="font-class p-10 fs-1 ">Welcome, {{firstName}}!</h2>

        <main class="container mt-4">
            <table class="table table-striped">
                <thead class="head-element fw-bold">
                    <tr>
                        <th scope="col">#</th>
                        <th scope="col">Name</th>
                        <th scope="col">Surname</th>
                        <th scope="col">Email</th>
                        <th scope="col">Role</th>
                    </tr>
                </thead>
                <tbody>
                    <tr v-for="(user,index) in users" :key="user.usrId">
                        <th class="fw-bold head-element" scope="row">{{index + 1}}</th>
                        <td>{{ user.firstName }}</td>
                        <td>{{ user.lastName }}</td>
                        <td>{{ user.email }}</td>
                        <td>{{ user.userRole }}</td>
                        <td>
                            <button class="btn btn-sm btn-danger"
                                    @click="deleteUser(user.usrId)">
                                Delete
                            </button>
                        </td>
                    </tr>
                </tbody>
            </table>
        </main>



    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import { apiFetch } from '../apiFetch';
    import { logout } from "../logout";
    import { useRouter } from 'vue-router';
    import { useToast } from "vue-toastification";

    const router = useRouter();
    const users = ref([]);
    const firstName = ref('');
    const index = ref(0);

    const toast = useToast();   

    onMounted(async () => {
        firstName.value = localStorage.getItem('firstName');

        try {
            const response = await apiFetch('http://localhost:5022/api/User/get_all');
            users.value = await response.json();
        } catch (err) {
            console.error('Error fetching users:', err);
        }
    });

    const deleteUser = async (id) => {
        try {
            const response = await fetch(`http://localhost:5022/api/User/${id}`, {
                method: 'DELETE',
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("token")}`,
                    "Content-Type": "application/json"
                }
            });
            if (response.ok) {
                users.value = users.value.filter(user => user.usrId !== id);
                toast.success('User deleted successfully');
            } else {
                const errorData = await response.json();
                toast.error('Error deleting user: ' + errorData.message);
            }
        } catch (err) {
            console.error("Error deleting user:", err);
        }
    };

    const handleLogout = () => {
        logout();
        router.push('/');
    }
</script>

<style scoped>

    .admin-page {
        background-color: cadetblue;
    }

    .sidebar {
        width: 300px;
    }


    .head-element {
        font-size: 1.2rem;
    }

    button:hover {
        background-color: darkmagenta;
    }


    .user-list {
        font-size: 1.2rem;
    }

    .manage-users {
        padding: 20px;
    }

    .soft-yellow {
        background-color: #FFF9DB; /* Soft pastel yellow */
    }

    .font-class {
        font-family: 'Book Antiqua';
    }
</style>