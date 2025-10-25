import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface AvailableBusDto {
  busScheduleId: string;
  companyName: string;
  busName: string;
  startTime: Date;
  arrivalTime: Date;
  seatsLeft: number;
  price: number;
  departureLocation: string;
  arrivalLocation: string;
  cancellationPolicy: string;
}

@Injectable({
  providedIn: 'root'
})
export class SearchService {

  private apiUrl = '/api/search';

  constructor(private http: HttpClient) { }

  searchAvailableBuses(from: string, to: string, journeyDate: string): Observable<AvailableBusDto[]> {

    let params = new HttpParams()
      .set('from', from)
      .set('to', to)
      .set('journeyDate', journeyDate);

    return this.http.get<AvailableBusDto[]>(`${this.apiUrl}/available-buses`, { params });
  }
}