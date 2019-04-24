<template>
  <v-container>
    <v-layout column>
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
            v-model="fixedAcidity"
            label="Fixed Acidity"
            required>
          </v-text-field>
        </v-flex>
        </v-layout>

        <v-flex xs4>
          <v-text-field
            v-model="volatileAcidity"
            label="Volatile Acidity"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
                  v-model="citricAcid"
                  label="Citric Acid"
                  required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="residualSugar"
            label="Residual Sugar"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="chlorides"
            label="Chlorides"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="freeSulfurDioxide"
            label="Free Sulfur Dioxide"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="totalSulfurDioxide"
            label="Total Sulfur Dioxide"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="density"
            label="Density"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="ph"
            label="Ph"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="sulphates"
            label="Sulphates"
            required>
          </v-text-field>
        </v-flex>

        <v-flex xs4>
          <v-text-field
            v-model="alcohol"
            label="Alcohol"
            required>
          </v-text-field>
        </v-flex>
      </v-form>
      <v-btn dark class="primary darken-2">
        Predict
      </v-btn>
    </v-layout>
  </v-container>
</template>

<script>
export default {
  name: "Predict",
  data: () => ({
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
    alcohol: 0
  })
  // methods: () => ({
  //   predict: () => {
  //     let prediction = {
  //       wineType: this.wineType,
  //       fixedAcidity: this.fixedAcidity,
  //       volatileAcidity: this.volatileAcidity,
  //       citricAcid: this.citricAcid,
  //       residualSugar: this.residualSugar,
  //       chlorides: this.chlorides,
  //
  //     }
  //   }
  // })
};
</script>

<style scoped></style>
