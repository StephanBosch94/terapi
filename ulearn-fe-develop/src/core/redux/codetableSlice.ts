import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { LoadingStatusEnum } from "../constants/enums/LoadingStatusEnum";
import { paths } from "../../apiTypes";
import { AppDispatch, RootState } from "./store";
import CodetableService from "../services/CodetableService";
import { ErrorResponse, ICodeTableObject } from "../services/Types/AppTypes";

export type EnvConfigurationResponse = paths['/api/env-configuration']['get']['responses']['200']['content']['application/json'];
export type CodetableCountryResponse = paths['/api/codetable/country']['get']['responses']['200']['content']['application/json'];
export type CodetableLanguageResponse = paths['/api/codetable/language']['get']['responses']['200']['content']['application/json'];
export type CodetableLanguageLevelResponse = paths['/api/codetable/language-level']['get']['responses']['200']['content']['application/json'];
export type CodetableSubjectsResponse = paths['/api/codetable/subject']['get']['responses']['200']['content']['application/json'];
export type CodetableSubjectLevelsResponse = paths['/api/codetable/subject-level']['get']['responses']['200']['content']['application/json'];
export type CodetableSpecializationsReponse = paths['/api/codetable/specialization']['get']['responses']['200']['content']['application/json'];
export type CodetableTeachingExperienceResponse = paths['/api/codetable/teaching-experience']['get']['responses']['200']['content']['application/json'];
export type CodetableWeeklyGoalReponse = paths['/api/codetable/weekly-goal']['get']['responses']['200']['content']['application/json'];
export type CodetableInterestsReponse = paths['/api/codetable/interest']['get']['responses']['200']['content']['application/json'];
export type CodetableReservationIssueTypes = paths['/api/codetable/reservation-issue-types']['get']['responses']['200']['content']['application/json'];

interface ICodetablesState {
    envConfigurations?: EnvConfigurationResponse | undefined,
    envConfigurationStatus?: LoadingStatusEnum | undefined,

    countries?: CodetableCountryResponse | undefined,
    countriesStatus?: LoadingStatusEnum | undefined,

    languages?: CodetableLanguageResponse | undefined,
    languageStatus?: LoadingStatusEnum | undefined,

    languageLevels?: CodetableLanguageLevelResponse | undefined,
    languageLevelsStatus?: LoadingStatusEnum | undefined,

    subjects?: CodetableSubjectsResponse | undefined,
    subjectsStatus?: LoadingStatusEnum | undefined,

    subjectLevels?: CodetableSubjectLevelsResponse | undefined,
    subjectLevelsStatus?: LoadingStatusEnum | undefined,

    specializations?: CodetableSpecializationsReponse | undefined,
    specilizationsStatus?: LoadingStatusEnum | undefined,

    teachingExperiences?: CodetableTeachingExperienceResponse | undefined,
    teachingExperiencesStatus?: LoadingStatusEnum | undefined,

    weeklyGoals?: CodetableWeeklyGoalReponse | undefined,
    weeklyGoalsStatus?: LoadingStatusEnum | undefined,

    interests?: CodetableInterestsReponse | undefined,
    interestsStatus?: LoadingStatusEnum | undefined,

    reservationIssueTypes?: CodetableReservationIssueTypes | undefined,
    reservationIssueTypesStatus?: LoadingStatusEnum | undefined,
}

interface ICodetablesMapped {
    envConfigurations: EnvConfigurationResponse
}

export const setEnvConfigurationsAsync = createAsyncThunk<ICodetablesMapped | ErrorResponse, {} | null | undefined, { dispatch: AppDispatch, state: RootState }>(
    "codetable/setEnvConfigurations",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getEnvConfiguration();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setCountriesAsync = createAsyncThunk<CodetableCountryResponse>(
    "codetable/setCountries",
    async (request, { rejectWithValue }) => {
        try {
            const countryResponse = await CodetableService.getCountries();
            return countryResponse.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setLanguagesAsync = createAsyncThunk<CodetableLanguageResponse>(
    "codetable/setLanguages",
    async (request, { rejectWithValue }) => {
        try {
            const languageResponse = await CodetableService.getLanguages();
            return languageResponse.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setLanguageLevelsAsync = createAsyncThunk<CodetableLanguageLevelResponse>(
    "codetable/setLanguageLevels",
    async (request, { rejectWithValue }) => {
        try {
            const languageLevelsResponse = await CodetableService.getLanguageLevels();
            return languageLevelsResponse.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setSubjectsAsync = createAsyncThunk<CodetableSubjectsResponse | ErrorResponse>(
    "codetable/setSubjects",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getSubjects();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setSubjectLevelsAsync = createAsyncThunk<CodetableSubjectLevelsResponse>(
    "codetable/setSubjectLevels",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getSubjectLevels();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setSpecializations = createAsyncThunk<CodetableSpecializationsReponse | ErrorResponse>(
    "codetable/setSpecializations",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getSpecializations();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setTeachingExperiences = createAsyncThunk<CodetableTeachingExperienceResponse | ErrorResponse>(
    "codetable/setTeachingExperiences",
    async (request, { rejectWithValue }) => {
        try {
            const languageLevelsResponse = await CodetableService.getTeachingExperiences();
            return languageLevelsResponse.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setWeeklyGoals = createAsyncThunk<CodetableWeeklyGoalReponse | ErrorResponse>(
    "codetable/setWeeklyGoals",
    async (request, { rejectWithValue }) => {
        try {
            const weeklyGoalsResponse = await CodetableService.getWeeklyGoals();
            return weeklyGoalsResponse.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setInterests = createAsyncThunk<CodetableInterestsReponse | ErrorResponse>(
    "codetable/setInterests",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getInterests();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const setReservationIssueTypes = createAsyncThunk<CodetableReservationIssueTypes | ErrorResponse>(
    "codetable/setReservationIssueTypes",
    async (request, { rejectWithValue }) => {
        try {
            const response = await CodetableService.getReservationIssueTypes();
            return response.data;
        }
        catch (e: any) {
            return rejectWithValue(e.response.data as ErrorResponse);
        }
    }
);

export const codeTableSlice = createSlice({
    name: "codeTableSlice",
    initialState: {} as ICodetablesState,
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(setEnvConfigurationsAsync.pending, (state) => {
            state.envConfigurationStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setEnvConfigurationsAsync.fulfilled,
            (state, action) => {
                state.envConfigurations = (action.payload as ICodetablesMapped).envConfigurations;
                state.envConfigurationStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setEnvConfigurationsAsync.rejected, (state) => {
            state.envConfigurationStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setCountriesAsync.pending, (state) => {
            state.countriesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setCountriesAsync.fulfilled,
            (state, action) => {
                state.countries = action.payload;
                state.countriesStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setCountriesAsync.rejected, (state) => {
            state.countriesStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setLanguagesAsync.pending, (state) => {
            state.languageStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setLanguagesAsync.fulfilled,
            (state, action) => {
                state.languages = action.payload;
                state.languageStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setLanguagesAsync.rejected, (state) => {
            state.languageStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setLanguageLevelsAsync.pending, (state) => {
            state.languageLevelsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setLanguageLevelsAsync.fulfilled,
            (state, action) => {
                state.languageLevels = action.payload;
                state.languageLevelsStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setLanguageLevelsAsync.rejected, (state) => {
            state.languageLevelsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setSubjectsAsync.pending, (state) => {
            state.subjectsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setSubjectsAsync.fulfilled,
            (state, action) => {
                state.subjects = action.payload as CodetableSubjectsResponse;
                state.subjectsStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setSubjectsAsync.rejected, (state) => {
            state.subjectsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setSubjectLevelsAsync.pending, (state) => {
            state.subjectLevelsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(
            setSubjectLevelsAsync.fulfilled,
            (state, action) => {
                state.subjectLevels = action.payload;
                state.subjectLevelsStatus = LoadingStatusEnum.Loaded;
            }
        );
        builder.addCase(setSubjectLevelsAsync.rejected, (state) => {
            state.subjectLevelsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setSpecializations.pending, (state) => {
            state.specilizationsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setSpecializations.fulfilled, (state, action) => {
            state.specializations = action.payload as CodetableSpecializationsReponse;
            state.specilizationsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setSpecializations.rejected, (state) => {
            state.specilizationsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setTeachingExperiences.pending, (state) => {
            state.teachingExperiencesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setTeachingExperiences.fulfilled, (state, action) => {
            state.teachingExperiences = action.payload as CodetableSpecializationsReponse;
            state.teachingExperiencesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setTeachingExperiences.rejected, (state) => {
            state.teachingExperiencesStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setWeeklyGoals.pending, (state) => {
            state.weeklyGoalsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setWeeklyGoals.fulfilled, (state, action) => {
            state.weeklyGoals = action.payload as CodetableWeeklyGoalReponse;
            state.weeklyGoalsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setWeeklyGoals.rejected, (state) => {
            state.weeklyGoalsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setInterests.pending, (state) => {
            state.interestsStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setInterests.fulfilled, (state, action) => {
            state.interests = action.payload as CodetableWeeklyGoalReponse;
            state.interestsStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setInterests.rejected, (state) => {
            state.interestsStatus = LoadingStatusEnum.Error;
        });
        // ------------------------------ //
        builder.addCase(setReservationIssueTypes.pending, (state) => {
            state.reservationIssueTypesStatus = LoadingStatusEnum.Loading;
        });
        builder.addCase(setReservationIssueTypes.fulfilled, (state, action) => {
            state.reservationIssueTypes = action.payload as CodetableReservationIssueTypes;
            state.reservationIssueTypesStatus = LoadingStatusEnum.Loaded;
        });
        builder.addCase(setReservationIssueTypes.rejected, (state) => {
            state.reservationIssueTypesStatus = LoadingStatusEnum.Error;
        });
    }
});

export const selectCountries = (state: RootState): CodetableCountryResponse | undefined => state.codetable.countries;
export const selectLanguages = (state: RootState): CodetableLanguageResponse | undefined => state.codetable.languages;
export const selectLanguageLevels = (state: RootState): CodetableLanguageLevelResponse | undefined => state.codetable.languageLevels;
export const selectSubjects = (state: RootState): CodetableSubjectsResponse | undefined => state.codetable.subjects;
export const selectSubjectLevels = (state: RootState): CodetableSubjectLevelsResponse | undefined => state.codetable.subjectLevels;
export const selectSubjectLevelsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.subjectLevelsStatus;
export const selectSpecializations = (state: RootState): CodetableSpecializationsReponse | undefined => state.codetable.specializations;
export const selectCountriesStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.countriesStatus;
export const selectLanguagesStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.languageStatus;
export const selectLanguageLevelsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.languageLevelsStatus;
export const selectSubjectsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.subjectsStatus;
export const selectSpecializationsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.specilizationsStatus;
export const selectTeachingExperiences = (state: RootState): CodetableTeachingExperienceResponse | undefined => state.codetable.teachingExperiences;
export const selectTeachingExperienceStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.teachingExperiencesStatus;
export const selectWeeklyGoals = (state: RootState): CodetableTeachingExperienceResponse | undefined => state.codetable.weeklyGoals;
export const selectWeeklyGoalsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.weeklyGoalsStatus;
export const selectInterests = (state: RootState): CodetableTeachingExperienceResponse | undefined => state.codetable.interests;
export const selectInterestsStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.interestsStatus;
export const selectReservationIssueTypes = (state: RootState): CodetableReservationIssueTypes | undefined => state.codetable.reservationIssueTypes;
export const selectReservationIssueTypesStatus = (state: RootState): LoadingStatusEnum | undefined => state.codetable.reservationIssueTypesStatus;

export default codeTableSlice.reducer;