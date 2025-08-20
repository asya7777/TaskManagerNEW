<template>
    <div class="homepage d-flex flex-column vh-100">
        <header class="navbar navbar-expand-lg navbar-light bg-light shadow-sm">
            <div class="container-fluid">
                <a class="navbar_brand fw_bold" href="/">
                    <img src="../assets/images/homeimg.png" alt="home image" width="50" height="50" />
                </a>

                <h1>TASK MANAGER</h1>
            </div>
        </header>

        <div class="d-flex to-do-list">
            <nav class="shadow-sm sidebar soft-yellow border-end p-3">
                <h2 class="to-do-text text-center fw-bold mb-3 fs-1">To do:</h2>
                <ul class="list-group list-group-flush">
                    <li v-for="task in assignedTasks"
                        :key="task.taskId"
                        class="list-group-item list-group-item-action"
                        role="button" @click="openTask(task)">
                        {{ task.taskName }}
                    </li>
                </ul>
            </nav>

            <main class="flex-grow-1 d-flex align-items-center justify-content-center text-center vh-100 ">

                <div class="d-flex align-items-center gap-5">
                    <span class=" fs-2 fw-bold">Create Task</span>
                    <button class="btn rounded-circle shadow d-flex align-items-center justify-content-center"
                            style="width: 80px; height: 80px; font-size: 4rem; "
                            @click="$router.push('/create-task')">
                        +
                    </button>
                </div>
            </main>

            <h2 class="font-class p-10 fs-1 ">Welcome, {{firstName}}!</h2>


        </div>

        <div v-if="selectedTask" class="modal-backdrop bg-white">
            <div class="modal-dialog-box">
                <div class="modal-header">
                    <h5>{{ selectedTask.taskName }}</h5>
                    <button @click="closeTask">X</button>
                </div>
                <div class="modal-body">
                    <p><strong>Description:</strong> {{ selectedTask.taskDescription }}</p>
                    <p><strong>Deadline:</strong> {{ selectedTask.taskDeadline }}</p>
                </div>
            </div>
        </div>

    </div>
</template>

<script setup>
    import { ref, onMounted } from 'vue';
    import CreateTask from './CreateTask.vue';
    import { apiFetch } from '../apiFetch';

    const assignedTasks = ref([]);
    const firstName = ref('');
    const selectedTask = ref(null);

    onMounted(async () => {
        firstName.value = localStorage.getItem('firstName');
        const userId = localStorage.getItem('userId');

        try {
            const response = await apiFetch(`http://localhost:5022/api/Task/my-tasks?userId=${userId}`);
            assignedTasks.value = await response.json();//pause until backend responds
        }catch (err) {
            console.error('Error fetching tasks:', err);
        }
    });``

    //modal açma ve kapama fonksiyonlarý
    const openTask = (task) => {
        selectedTask.value = task;
    };
    const closeTask = () => {
        selectedTask.value = null;
    };

</script>

<style scoped>
    .sidebar {
        width: 300px;
    }
    .soft-yellow {
        background-color: #FFF9DB; /* Soft pastel yellow */
    }
    .font-class{
        font-family:'Book Antiqua';
    }
    button {
        background-color: mediumseagreen;
    }
    button:hover {
            background-color: seagreen;
        }
    .to-do-list{
        font-family:72;
    }
    .to-do-text{
        text-decoration:underline;
    }
</style>