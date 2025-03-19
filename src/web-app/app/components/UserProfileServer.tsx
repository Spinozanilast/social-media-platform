// import IdentityService from '@api/services/user';
// import ProfileService from '@api/services/profile';
// import Profile from '@/app/api/types/profiles/profile';
//
// export const fetchIsAuthenticated = async (
//     userName: string,
//     setIsAuthenticated: (isAuthenticated: boolean) => void
// ) => {
//     const isUserAuthenticated =
//         await IdentityService.checkUserIdentity(userName);
//     setIsAuthenticated(isUserAuthenticated);
// };
//
// export const fetchProfileImage = async (
//     userId: string,
//     setCurrentProfileImage: (image: Blob | null) => void
// ) => {
//     try {
//         const profileImage = await ProfileService.getProfileImage(userId);
//         setCurrentProfileImage(profileImage);
//     } catch (error) {
//         console.error('Error fetching profile image:', error);
//         setCurrentProfileImage(null);
//     }
// };
//
// export const handleSaveProfile = async (
//     updatedProfile: Profile,
//     userId: string,
//     setCurrentProfileInfo: (profile: Profile) => void
// ) => {
//     if (
//         updatedProfile.birthDate &&
//         !isValidDateFormat(updatedProfile.birthDate.toString())
//     ) {
//         updatedProfile.birthDate = formatDate(
//             updatedProfile.birthDate.toString()!
//         );
//     }
//     try {
//         await ProfileService.updateProfile(updatedProfile, userId);
//         setCurrentProfileInfo(updatedProfile);
//         console.log('Profile updated:', updatedProfile);
//     } catch (error) {
//         console.error('Error updating profile:', error);
//     }
// };
//
// export const handleImageUpload = async (
//     uploadedImage: Blob,
//     userId: string,
//     setCurrentProfileImage: (image: Blob) => void
// ) => {
//     try {
//         const file = new File([uploadedImage], 'profileImage', {
//             type: uploadedImage.type,
//         });
//         await ProfileService.uploadProfileImage(file, userId);
//         setCurrentProfileImage(uploadedImage);
//         console.log('Image uploaded:', uploadedImage);
//     } catch (error) {
//         console.error('Error uploading image:', error);
//     }
// };
//
// const isValidDateFormat = (dateString: string) => {
//     const regex = /^\d{4}-\d{2}-\d{2}$/;
//     return regex.test(dateString);
// };
//
// const formatDate = (dateString: string) => {
//     const date = new Date(dateString);
//     return date.toISOString().split('T')[0];
// };
