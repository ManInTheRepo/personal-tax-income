<template>
  <div class="mx-auto max-w-4xl p-4">
    <header class="mb-6">
      <h1>PTI — Personal Tax Income</h1>
      <nav style="display:flex; gap:12px; margin-top:8px">
        <router-link to="/">Dashboard</router-link>
        <router-link to="/income">Income</router-link>
        <router-link to="/summary">Summary</router-link>
      </nav>
      <hr />
    </header>
    <router-view />
  </div>
</template>

<script setup lang="ts"></script>

<style>
body { font-family: system-ui, -apple-system, Segoe UI, Roboto, Helvetica, Arial, sans-serif }
a { text-decoration: none }
a.router-link-active { font-weight: 600 }
</style>
