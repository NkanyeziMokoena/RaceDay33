using System.ComponentModel.DataAnnotations;
namespace raceday_mvc.Models;
public class LoginVm { [Required, EmailAddress] public string Email {get;set;}=""; [Required,DataType(DataType.Password)] public string Password {get;set;}=""; }
public class RegisterVm { [Required] public string FullName {get;set;}=""; [Required,EmailAddress] public string Email {get;set;}=""; [Required,MinLength(8),DataType(DataType.Password)] public string Password {get;set;}=""; [Required] public string Role {get;set;}="Participant"; }
public class EventVm { public int EventId {get;set;} public int OrganiserId {get;set;} public string Name {get;set;}=""; public string? Description {get;set;} public DateTime EventDate {get;set;} public string Location {get;set;}=""; public string EventType {get;set;}="Run"; public decimal? DistanceKm {get;set;} }
public class CategoryVm { public int CategoryId {get;set;} public int EventId {get;set;} public string Name {get;set;}=""; public decimal DistanceKm {get;set;} public int MaxParticipants {get;set;} }
public class EnrolmentVm { public int EnrolmentId {get;set;} public int ParticipantId {get;set;} public string ParticipantName {get;set;}=""; public int CategoryId {get;set;} public string CategoryName {get;set;}=""; public int EventId {get;set;} public string EventName {get;set;}=""; public DateTime EnrolmentDate {get;set;} public string BibNumber {get;set;}=""; public string Status {get;set;}=""; }
public class ResultVm { public int ResultId {get;set;} public int EnrolmentId {get;set;} public int CategoryId {get;set;} public string ParticipantName {get;set;}=""; public string BibNumber {get;set;}=""; public TimeSpan? FinishTime {get;set;} public int? Position {get;set;} public string Status {get;set;}=""; }

public class ProfileVm { [Required] public string FullName {get;set;}=""; [Required,EmailAddress] public string Email {get;set;}=""; }
