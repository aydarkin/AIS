import Vue from 'vue'
import App from './App.vue'
import Buefy from 'buefy'
import 'buefy/dist/buefy.css'
import VueRouter from 'vue-router'

Vue.config.productionTip = false
Vue.use(Buefy)
Vue.use(VueRouter)

const router = new VueRouter({
  routes: [
    {
      path: '/', component: App,
      
    }
  ],
  mode: "history"
})


new Vue({
  router,
  render: h => h(App),
}).$mount('#app')
