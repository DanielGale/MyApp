

new Vue({
    el: '#root',
    data: {
        states: []
    },
    mounted() {
        axios.get('https://localhost:5001/api/states')
            .then(response => this.states = response.data);
    }
});