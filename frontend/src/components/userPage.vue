<template>
    <div class="user-page d-flex flex-column vh-100">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="/">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50" />
                </a>
                <button class="btn" @click="handleLogout">Logout</button>
            </div>
        </header>

        <div class="d-flex">
            <nav class="shadow-sm sidebar soft-yellow border-end p-3">
                <h2 class="to-do-text text-center fw-bold mb-3 fs-1">TO DO:</h2>
                <p v-if="error" class=" fw-bold error-message text-danger text-center">{{ error }}</p>
                <ul class="to-do-list list-group list-group-flush">
                    <li v-for="task in assignedTasks"
                        :key="task.taskId"
                        class="task-titles fw-bold list-group-item list-group-item-action">
                        <input type="checkbox"
                               v-model="task.isFinished"
                               @change="markAsFinished(task)"
                               class="me-2" />
                        <span :class="{ 'text-decoration-line-through': task.isFinished }"
                              @click="openTask(task)"
                              style="cursor: pointer;">
                            {{ task.taskName }}
                        </span>
                    </li>
                </ul>
            </nav>

            <div class="d-flex flex-column">
                <button class="sort-btn btn mb-3" @click="showFilterPopup = true">
                    Sort By
                </button>
                <button class="sort-btn btn" @click="refresh">Refresh</button>
            </div>

            <main class="user-main flex-grow-1 d-flex align-items-center justify-content-center text-center vh-100 ">

                <div class="d-flex align-items-center gap-5">
                    <span class="fs-2 fw-bold">Create Task</span>
                    <button class="btn rounded-circle shadow d-flex align-items-center justify-content-center"
                            style="width: 80px; height: 80px; font-size: 4rem; "
                            @click="$router.push('/create-task')">
                        +
                    </button>
                </div>
            </main>

            <h2 class="font-class p-10 fs-1 ">Welcome, {{firstName}}!</h2>


        </div>

        <div v-if="selectedTask" class="modal-backdrop ">
            <div class=" modal-dialog-box">
                <div class="modal-header">
                    <h4 class="mb-5">{{ selectedTask.taskName }}</h4>
                    <button @click="closeTask">X</button>
                </div>
                <div class="modal-body">
                    <p><strong>Description:</strong> {{ selectedTask.taskDescription }}</p>
                    <p><strong>Deadline:</strong> {{ selectedTask.taskDeadline }}</p>
                    <ul class="tag-list mb-5">
                        <li class="tag-pill" v-for="tag in selectedTask.tags">
                            {{tag.tagName}}
                        </li>
                    </ul>
                </div>
                <!--<button class="delete-button btn btn-sm" @click="deleteTask(selectedTask.taskId)">Delete</button>-->
            </div>
        </div>

        <div v-if="showFilterPopup" class="modal-backdrop">
            <div class="modal-dialog-box">
                <div class="modal-header">
                    <h4 class="mb-5">Sort Tasks</h4>
                    <button class="mb-5" @click="showFilterPopup=false">X</button>
                </div>
                <div class="modal-body">
                    <button class="btn w-100 mb-10" @click="sortByDeadline">Sort by urgency</button>

                    <div>
                        <input type="text" v-model="tagFilter" class="form-control mb-2" placeholder="Enter tag name" />
                        <button class="btn w-100" @click="sortByTag">Filter by tag</button>
                    </div>
                </div>
            </div>
        </div>

        <div v-if="loading" class="loading-overlay">
            <div class="spinner-border" role="status">
                <span class="visually-hidden"> Loading...</span>
            </div>
        </div>

    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import CreateTask from './CreateTask.vue';
    import { apiFetch } from '../apiFetch';
    import { useRouter } from 'vue-router';
    import { logout } from "../logout";

    const router = useRouter();

    const error = ref('');

    const allTasks = ref([]);
    const assignedTasks = ref([]);
    const firstName = ref('');
    const selectedTask = ref(null);

    const showFilterPopup = ref(false);
    const tagFilter = ref('');

    const loading = ref(false);

    const fetchTasks = async () => {
        const userId = localStorage.getItem('userId');
        loading.value = true;

        try {
            const response = await apiFetch(`http://localhost:5022/api/Task/my-tasks?userId=${userId}`);
            const data = await response.json();
            allTasks.value = data;
            assignedTasks.value = [...data];
        } catch (err) {
            console.error('Error fetching tasks:', err);
            error.value = 'Server might not be running!';
            allTasks.value = [];
        } finally {
            loading.value = false;
        }
    };

    onMounted(async () => {
        firstName.value = localStorage.getItem('firstName');

        await fetchTasks();
    });

    //modal açma ve kapama fonksiyonlarý
    const openTask = (task) => {
        selectedTask.value = task;
    };
    const closeTask = () => {
        selectedTask.value = null;
    };
    const refresh = (async () => {
        await fetchTasks();
    });

    const sortByDeadline = () => {
        assignedTasks.value.sort((a, b) => new Date(a.taskDeadline) - new Date(b.taskDeadline));
        showFilterPopup.value = false;
    };

    const sortByTag = () => {
        if (!tagFilter.value) return;

        assignedTasks.value = allTasks.value.filter(
            (task) => task.tags?.some(tag => tag.tagName.toLowerCase() === tagFilter.value.toLowerCase()));

        showFilterPopup.value = false;
    };

    const markAsFinished = async (task) => {
        try {
            await apiFetch(`http://localhost:5022/api/Task/${task.taskId}/finish-task`, {
                method: "PUT",
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("token")}`,
                    "Content-Type": "application/json"
                },
                body: JSON.stringify({ isFinished: task.isFinished })//sends isFinished depending on the checkbox state
            });


        } catch (err) {
            console.error("Error finishing task:", err);
            task.isFinished = !task.isFinished;
        }
    };

    const deleteTask = async (taskId) => {
        try {
            const response = await fetch(`http://localhost:5022/api/Task/${taskId}`, {
                method: 'DELETE',
                headers: {
                    "Authorization": `Bearer ${localStorage.getItem("token")}`,
                    "Content-Type": "application/json"
                }
            });
            if (response.ok) {
                allTasks.value = allTasks.value.filter(task => task.taskId !== taskId);
                assignedTasks.value = assignedTasks.value.filter(task => task.taskId !== taskId);
                alert('Task deleted successfully');
                selectedTask.value = null;
            } else {
                const errorData = await response.json();
                alert('Error deleting task: ' + errorData.message);
            }
        } catch (err) {
            console.error("Error deleting task:", err);
        }

    };


    const handleLogout = () => {
        logout();
        router.push('/');
    };
</script>

<style scoped>
    .user-page {
        background-image: url('../assets/images/background-user.jpg');
        background-size: cover;
    }

    .sidebar {
        width: 300px;
    }

    .sort-btn {
        height: 6%;
    }

    .soft-yellow {
        background-color: #FFF9DB; /* Soft pastel yellow */
    }

    .font-class {
        font-family: 'Book Antiqua';
    }

    button {
        background-color: mediumseagreen;
    }

        button:hover {
            background-color: darkmagenta;
        }

    .modal-body {
        font-size: 1.1rem;
    }

    .delete-button {
        font-size: 1.2rem;
    }

    .tag-pill {
        display: inline-flex;
        width: 15%;
        padding: 1px 1px 2px 5px;
        align-items: center;
        gap: 6px;
        font-size: 1rem;
        margin: 4px;
        background-color: #e0f7fa;
        border-radius: 20px;
        font-weight: 550;
    }

    .tag-list {
        list-style: none;
    }

    .to-do-list {
        font-family: 72;
        font-size: 1.2rem;
    }

    .to-do-text {
        text-decoration: underline;
    }

    .modal-backdrop {
        width: 100%;
        height: 100%;
        background: rgba(0, 0, 0, 0.5);
        display: flex;
        align-items: center;
        justify-content: center;
    }

    .modal-dialog-box {
        background: white;
        border-radius: 8px;
        padding: 20px;
        max-width: 500px;
        width: 90%; /* responsive */
        box-shadow: 0 2px 10px rgba(0,0,0,0.2);
    }

    .loading-overlay {
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: rgba(255, 255, 255, 0.7);
        display: flex;
        align-items: center;
        justify-content: center;
        z-index: 2000;
    }

    .error-message {
        font-size: 1.2rem;
    }
</style>