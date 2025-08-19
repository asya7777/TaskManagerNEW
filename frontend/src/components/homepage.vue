<template>
    <div class="homepage d-flex flex-column vh-100">
        <header class="bg-success shadow-sm py-3 text-center">
            <h1 class="fw-bold m-0">TASK MANAGER</h1>
        </header>

        <div class="d-flex">
            <nav class="sidebar bg-white border-end p-3">
                <h2 class="fw-bold mb-3 fs-1">To do:</h2>
                <ul class="list-group list-group-flush">
                    <li v-for="task in assignedTasks"
                        :key="task.taskId"
                        class="list-group-item list-group-item-action"
                        role="button" @click="openTask(task)">
                        {{ task.taskName }}
                    </li>
                </ul>
            </nav>

            <h2 class="p-10 fs-1">Welcome, {{firstName}}</h2>

            <main class="flex-grow-1 d-flex align-items-center justify-content-center text-center vh-100">

                <div class="d-flex align-items-center gap-5">
                    <span class="fs-3 fw-semibold">Create Task</span>
                    <button class="btn btn-primary rounded-circle shadow d-flex align-items-center justify-content-center"
                            style="width: 60px; height: 60px; font-size: 1.5rem;"
                            @click="$router.push('/create-task')">
                        +
                    </button>
                </div>
            </main>
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

        try {
            const response = await apiFetch('http://localhost:5022/api/Task/my-tasks');
            assignedTasks.value = await response.json();//pause until backend responds
        }catch (err) {
            console.error('Error fetching tasks:', err);
        }
    });

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
</style>