export const validateEmail = (email) => {
  let re =
    /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;

  if (re.test(email)) {
    return true;
  } else {
    return false;
  }
};

// https://stackoverflow.com/questions/39356826/how-to-check-if-it-a-text-input-has-a-valid-email-format-in-reactjs

export const validatePword = (password) => {
  var lowerCaseLetters = /[a-z]/g;
  var upperCaseLetters = /[A-Z]/g;
  var numbers = /[0-9]/g;

  // if (password.match(lowerCaseLetters))
  return password.match(lowerCaseLetters);
};
