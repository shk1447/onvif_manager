<template>
  <video controls autoplay class="rtsp-video" ref="video" :src="url" />
</template>

<script lang="ts">
import Vue from 'vue';
interface IRtspVideo {
  url: string;
}

export default Vue.extend<IRtspVideo, any, any, any>({
  mounted() {
    console.log(this.url);
  },
});
</script>

<style scoped>
.rtsp-video {
  width: 100%;
  height: 100%;
}
</style>
