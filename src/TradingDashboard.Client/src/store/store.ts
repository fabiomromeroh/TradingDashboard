import {createSlice, configureStore} from '@reduxjs/toolkit';

const userSlice = createSlice({
    name: 'user',
    initialState: {
        name: 'John Doe',
        email: ''
    },
    reducers:{
        setUser(state, action) {
            state.name = action.payload.name;
            state.email = action.payload.email;
        }
    }});

const store = configureStore({
    reducer: {
        user: userSlice.reducer
    }
})

export const {setUser} = userSlice.actions;
export default store;