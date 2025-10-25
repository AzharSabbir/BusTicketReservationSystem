import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface SeatDto {
  seatId: string;
  seatNumber: string;
  status: string;
}

export interface SeatPlanDto {
  busScheduleId: string;
  busName: string;
  seats: SeatDto[];
}

export interface BookSeatInputDto {
  busScheduleId: string;
  seatId: string;
  passengerName: string;
  passengerMobile: string;
  boardingPoint: string;
  droppingPoint: string;
}

export interface BookSeatResultDto {
  success: boolean;
  message: string;
  ticketId?: string;
  seatNumber?: string;
  status?: string;
}

@Injectable({
  providedIn: 'root'
})
export class BookingService {

  private apiUrl = '/api/Booking';

  constructor(private http: HttpClient) { }

  getSeatPlan(scheduleId: string): Observable<SeatPlanDto> {
    return this.http.get<SeatPlanDto>(`${this.apiUrl}/seat-plan/${scheduleId}`);
  }

  bookSeat(input: BookSeatInputDto): Observable<BookSeatResultDto> {
    return this.http.post<BookSeatResultDto>(`${this.apiUrl}/book-seat`, input);
  }
}