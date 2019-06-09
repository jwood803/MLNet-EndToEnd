<template>
  <v-container>
    <v-layout column v-if="!loaded">
      <v-form v-model="valid">
        <v-layout row>
        <v-flex xs4>
          <v-select
            v-model="wineType"
            :items="wineTypes"
            label="Wine type"
            required>
          </v-select>

          <v-text-field
            v-model.number="fixedAcidity"
            label="Fixed Acidity"
            required>
          </v-text-field>
        </v-flex>
        </v-layout>

        <v-flex xs4>
          <v-text-field
            v-model.number="volatileAcidity"
            label="Volatile Acidity"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="citricAcid"
            label="Citric Acid"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="residualSugar"
            label="Residual Sugar"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="chlorides"
            label="Chlorides"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="freeSulfurDioxide"
            label="Free Sulfur Dioxide"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="totalSulfurDioxide"
            label="Total Sulfur Dioxide"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="density"
            label="Density"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="ph"
            label="Ph"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="sulphates"
            label="Sulphates"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model.number="alcohol"
            label="Alcohol"
            required>
          </v-text-field>
        </v-flex>
      </v-form>
      <v-btn @click="predict" dark class="primary darken-2">
        Predict
      </v-btn>
    </v-layout>
    <div v-if="loaded">
      <div>
        Prediction is {{prediction | number}}
      </div>
      <div>
        <a @click="resetPrediction">Another prediction</a>
      </div>
    </div>
  </v-container>
</template>

<script>
import axios from "axios";

export default {
  name: "Predict",
  data () {
    return {
      valid: true,
      wineTypes: ["Red", "White"],
      wineType: "",
      fixedAcidity: 0,
      volatileAcidity: 0,
      citricAcid: 0,
      residualSugar: 0,
      chlorides: 0,
      freeSulfurDioxide: 0,
      totalSulfurDioxide: 0,
      density: 0,
      ph: 0,
      sulphates: 0,
      alcohol: 0,
      prediction: null,
      loaded: false,
      error: null
    }
  },
  methods: {
    predict() {
      const prediction = {
        wineType: this.wineType,
        fixedAcidity: this.fixedAcidity,
        volatileAcidity: this.volatileAcidity,
        citricAcid: this.citricAcid,
        residualSugar: this.residualSugar,
        chlorides: this.chlorides,
        freeSulfurDioxide: this.freeSulfurDioxide,
        totalSulfurDioxide: this.totalSulfurDioxide,
        density: this.density,
        ph: this.ph,
        sulphates: this.sulphates,
        alcohol: this.alcohol
      };

      axios.post("http://localhost:59223/api/model", prediction)
        // eslint-disable-next-line
        .then(resp => {
          this.prediction = resp.data;
        })
        .catch(err => this.error = err)
        .finally(() => {
          this.loaded = true;
        });
    },
    resetPrediction() {
      this.loaded = false;
    }
  },
  filters: {
    number(value) {
      return value.toPrecision(2);
    }
  }
};
</script>

<style scoped></style>
