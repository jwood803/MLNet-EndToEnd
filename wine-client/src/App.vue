<template>
  <div id="app">
    <v-app dark>
      <Toolbar></Toolbar>
      <v-content>
        <router-link></router-link>
      </v-content>
    </v-app>
  </div>
</template>

<script>
import Toolbar from "./components/Toolbar";

export default {
  name: 'app',
  components: {Toolbar}
}
</script>

<style>

</style>
