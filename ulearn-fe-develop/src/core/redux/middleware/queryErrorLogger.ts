import {isRejected, isRejectedWithValue, Middleware, MiddlewareAPI} from "@reduxjs/toolkit";
import Logger from "../../services/Utils/Logger";

export const queryErrorLogger: Middleware =
    (api: MiddlewareAPI) => (next) => (action) => {
        // RTK Query uses `createAsyncThunk` from redux-toolkit under the hood, so we're able to utilize these matchers!
        if (isRejectedWithValue(action)) {
            Logger.error(action.error)
        }
        if (isRejected(action)){
            Logger.error(action.error)
        }
        return next(action)
    }