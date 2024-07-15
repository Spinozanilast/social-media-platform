const isEmailValid = (email: string): boolean => {
    const emailRegex: RegExp = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/;
    return emailRegex.test(email);
};

export default isEmailValid;
