

new Vue({
    el: '#root',
    data: {
        states: []
    },
    mounted() {
        axios.interceptors.request.use(
            (config) => {
                let token = localStorage.getItem('authtoken');

                if (token) {
                    config.headers['Authorization'] = `Bearer ${token}`;
                }

                return config;
            },

            (error) => {
                return Promise.reject(error);
            }
        );

        axios.get('https://localhost:5001/api/states')
            .then(response => this.states = response.data);
    }
});