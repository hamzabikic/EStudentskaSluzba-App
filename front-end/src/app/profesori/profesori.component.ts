import { Component, OnInit } from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {MojConfig} from "../MojConfig";

@Component({
  selector: 'app-profesori',
  templateUrl: './profesori.component.html',
  styleUrls: ['./profesori.component.css']
})
export class ProfesoriComponent implements OnInit {

  nastavnici:any;
  nastavnici_copy:any;
  imeprezime ="";
  otvori = false;
  ime ="";
  prezime ="";
  datumRodjenja = new Date();
  opstinaId =0;
  email ="";
  korisnickoIme="";
  datumZaposlenja = new Date();
  zvanje ="";
  opstine:any;
  dialog_novi_profesor = false;
  moguce_slanje = true;

  constructor(private http : HttpClient) { }

  async ngOnInit() {
    await this.ucitajProfesore();
    await this.ucitajOpstine();
  }
  async dodajStudenta() {
    let obj = {
      ime: this.ime,
      prezime:this.prezime,
      datumRodjenja: this.datumRodjenja,
      opstinaId:this.opstinaId,
      korisnickoIme:this.korisnickoIme,
      email:this.email,
      datumZaposlenja:this.datumZaposlenja,
      zvanje:this.zvanje
    };
    this.moguce_slanje = false;
    let res = await this.http.post(MojConfig.adresa_servera+"/Nastavnik/addNastavnik",obj).toPromise();
    // @ts-ignore
    if(res){
      // @ts-ignore
      alert("Uspjesno dodan profesor!");
      this.moguce_slanje = true;
      await this.ucitajProfesore();
      this.pretraga();
      this.obrisiPodatke();
      return;
    }
    alert("Neuspjesno dodavanje profesora!");
    this.moguce_slanje = true;
  }

  async ucitajOpstine() {
    this.opstine = await this.http.get(MojConfig.adresa_servera +"/Opstina/getOpstine").toPromise();
  }
  async ucitajProfesore() {
    this.nastavnici = await this.http.get(MojConfig.adresa_servera + "/Nastavnik/getNastavnici")
      .toPromise();
    this.nastavnici_copy = this.nastavnici;
  }
  async obrisiStudenta(id:number) {
    let res = await this.http.delete(MojConfig.adresa_servera+"/Nastavnik/deleteNastavnik?id="+ id).toPromise();
    if(res) {
      alert("Uspjesno obrisan profesor!");
      await this.ucitajProfesore();
      this.pretraga();
      return;
    }
    alert("Neuspjesno brisanje profesora!");
  }
  pretraga() {
    this.nastavnici_copy = this.nastavnici.filter((s:any)=> (s.ime + " " + s.prezime).toLowerCase().toLowerCase()
      .includes(this.imeprezime.toLowerCase()));
  }

  protected readonly Date = Date;

  obrisiPodatke() {
    this.ime ="";
    this.prezime ="";
    this.datumZaposlenja = new Date();
    this.zvanje ="";
    this.opstinaId =0;
    this.email ="";
    this.korisnickoIme ="";
    this.datumRodjenja = new Date();
  }

}
