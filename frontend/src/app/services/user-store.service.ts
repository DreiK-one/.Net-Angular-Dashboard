import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserStoreService {

  private _fullName$ = new BehaviorSubject<string>("");
  private _role$ = new BehaviorSubject<string>("");

  constructor() { }

  public getRoleFromStore(){
    return this._role$.asObservable();
  }

  public setRoleForStore(role: string){
    this._role$.next(role);
  }

  public getFullNameFromStore(){
    return this._fullName$.asObservable();
  }

  public setFullNameForStore(fullname: string){
    this._fullName$.next(fullname);
  }
}
