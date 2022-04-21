<template>
  <video controls autoplay class="rtsp-video" ref="player" />
</template>

<script lang="ts">
import Vue from 'vue';
import flvjs from 'flv.js';
interface IRtspVideo {
  name: string;
  hostname: string;
  username: string;
  password: string;
  type: string;
  stream_url: string;
}

export default Vue.extend<IRtspVideo, any, any, any>({
  created() {
    console.log(this.name);
    this.$electron.send('rtsp/start', {
      name: this.name,
      stream_url: this.stream_url,
      username: this.username,
      password: this.password,
    });
  },
  mounted() {
    if (flvjs.isSupported()) {
      let video = this.$refs.player;
      if (video) {
        this.player = flvjs.createPlayer({
          type: 'flv',
          isLive: true,
          url: `ws://localhost:9090/rtsp/${this.name}`,
        });
        this.player.attachMediaElement(video);
        try {
          this.player.load();
          this.player.play();
        } catch (error) {
          console.log(error);
        }
      }
    }
  },
  destroyed() {
    console.log('destroy:!@!');
    this.$electron.send('rtsp/stop', {
      name: this.name,
      stream_url: this.stream_url,
      username: this.username,
      password: this.password,
    });
  },
});
</script>

<style scoped>
.rtsp-video {
  width: 100%;
  height: 100%;
}
</style>
