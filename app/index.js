import React from "react";
import {
  StyleSheet, Text, View, AsyncStorage, Button,
} from "react-native";
import CustomOnbarding from "./screens/CustomOnbarding";
import MetroMap from "./screens/MetroMap";

const storageKeys = {
  showOnboarding: "showOnboarding",
};

const styles = StyleSheet.create({
  container: {
    flex: 1,
    backgroundColor: "#fff",
  },
});

export default class App extends React.Component {
  state = {
    showOnboarding: true,
  };

  async componentDidMount() {
    await this.getShowOnBoarding();
  }

  onDone = async () => {
    this.setState(
      () => ({
        showOnboarding: false,
      }),
      async () => this.saveShowOnBoarding(false),
    );
  };

  saveShowOnBoarding = async (value) => {
    try {
      await AsyncStorage.setItem(storageKeys.showOnboarding, JSON.stringify(value));
    } catch (error) {
      console.error(error);
    }
  };

  getShowOnBoarding = async () => {
    try {
      const value = await AsyncStorage.getItem(storageKeys.showOnboarding);
      if (value) {
        this.setState({
          showOnboarding: JSON.parse(value),
        });
      }
    } catch (err) {
      console.error(err);
    }
  };

  // For development
  clearAsyncStorage = async () => {
    AsyncStorage.clear();
  };

  render() {
    const { showOnboarding } = this.state;
    if (showOnboarding) {
      return <CustomOnbarding onDone={this.onDone} />;
    }

    return (
      <View style={styles.container}>
        <MetroMap />
        <Button onPress={this.clearAsyncStorage} title="clear">
          <Text>Clear Async Storage</Text>
        </Button>
      </View>
    );
  }
}
