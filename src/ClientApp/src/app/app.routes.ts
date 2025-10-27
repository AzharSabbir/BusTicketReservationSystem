import { Routes } from '@angular/router';
import { SearchComponent } from './search/search';
import { SeatPlan } from './seat-plan/seat-plan';

export const routes: Routes = [
    {
        path: '',
        component: SearchComponent
    },
    {
        path: 'seat-plan/:scheduleId',
        component: SeatPlan
    },
];