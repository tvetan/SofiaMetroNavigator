import React from "react";
import { Image } from "react-native";
import Onboarding from "react-native-onboarding-swiper";
import PropTypes from "prop-types";

const SimpleOnboarding = ({ onDone }) => (
  <Onboarding
    pages={[
      {
        backgroundColor: "#fff",
        image: <Image source={require("../res/images/first.jpg")} />,
        title: "Useful",
        subtitle: "Never miss your train",
      },
      {
        backgroundColor: "#fe6e58",
        image: <Image source={require("../res/images/second.jpg")} />,
        title: "Easy",
        subtitle: "Find the station that is closest to you.",
      },
      {
        backgroundColor: "#999",
        image: <Image source={require("../res/images/third.jpg")} />,
        title: "Organized",
        subtitle: "Organize your trip in Sofia",
      },
    ]}
    onDone={onDone}
  />
);

SimpleOnboarding.propTypes = {
  onDone: PropTypes.func.isRequired,
};

export default SimpleOnboarding;
