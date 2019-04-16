<template>
    <v-toolbar>
        <v-toolbar-title>Wine Quality</v-toolbar-title>
        <v-spacer></v-spacer>
        <v-toolbar-items class="hidden-sm-and-down">
            <router-link to="/" tag="v-btn">Predict</router-link>
            <router-link to="/data" tag="v-btn">Data</router-link>
        </v-toolbar-items>
    </v-toolbar>
</template>

<script>
  export default {
    name: "Toolbar"
  };
</script>

<style scoped>
button.router-link-exact-active {
    border-bottom: 1px solid #f66;
}
</style>
