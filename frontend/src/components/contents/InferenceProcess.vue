<template>
  <div style="width: 100%; height: 100%" ref="data_table">
    <v-system-bar window height="36" color="rgb(42,42,42)">
      <v-icon small>mdi-information-outline</v-icon>
      <span>you can start/stop/manage process.</span>
      <v-spacer></v-spacer>
      <v-icon color="succss" small @click="() => {}">mdi-plus-thick</v-icon>
    </v-system-bar>
    <v-data-table
      v-if="headers.length > 0"
      fixed-header
      :height="table_height + 'px'"
      class="data-table"
      :headers="headers"
      :items="data_rows"
      dense
      hide-default-footer
      :items-per-page="-1"
    >
      <template v-slot:body="{ items }">
        <tbody>
          <tr v-for="(item, idx) in items" :key="idx" @click="() => {}">
            <td v-for="header in headers" :key="header.value">
              <div
                v-if="header.value"
                :style="
                  header.disabled ? 'pointer-events:none;opacity:.5;' : ''
                "
              >
                <v-icon v-if="header.type == 'text' || header.type == 'json'">
                  mdi-dots-horizontal
                </v-icon>
                <input
                  type="checkbox"
                  v-model="item[header.value]"
                  :name="header.value"
                  v-else-if="header.type == 'checkbox'"
                  @change="() => {}"
                />
                <select
                  v-else-if="header.type == 'select'"
                  class="field-type-select"
                  v-model="item[header.value]"
                  @change="() => {}"
                >
                  <option
                    v-for="(option, k) in header.options.items"
                    :key="k"
                    :value="option.value"
                  >
                    {{ option.text }}
                  </option>
                </select>
                <div v-else-if="header.type == 'file'">
                  <v-chip
                    v-if="item[header.value]"
                    class="ma-2"
                    close
                    color="rgba(222,222,222,0.8)"
                    label
                    outlined
                    small
                    @click="() => {}"
                    @click:close="() => {}"
                  >
                    {{ item[header.value] }}
                  </v-chip>
                  <input
                    v-else
                    :type="'file'"
                    :class="'table-edit-input file'"
                    v-model="item[header.value]"
                    @change="() => {}"
                  />
                </div>
                <input
                  v-else
                  :type="'text'"
                  :class="'table-edit-input text'"
                  v-model="item[header.value]"
                  @change="() => {}"
                />
              </div>
              <div
                v-else
                style="
                width:
                display: flex;
                justify-content: center;
                align-items: center;
              "
              >
                <v-icon small color="orange" @click="() => {}"
                  >mdi-pencil</v-icon
                >
                <v-icon small color="red" @click="() => {}"
                  >mdi-close-thick</v-icon
                >
              </div>
            </td>
          </tr>
        </tbody>
      </template>
    </v-data-table>
  </div>
</template>

<script lang="ts">
import Vue from 'vue';
export default Vue.extend<any, any, any, any>({
  data() {
    return {
      headers: [
        {
          type: 'text',
          value: 'id',
          text: 'PID',
        },
        {
          type: 'text',
          value: 'ip',
          text: 'IP',
        },
        {
          type: 'text',
          value: 'port',
          text: 'PORT',
        },
        {
          type: 'text',
          value: 'status',
          text: 'STATUS',
        },
        {
          value: 'action',
          text: 'ACTION',
          sortable: false,
        },
      ],
      data_rows: [],
    };
  },
});
</script>
