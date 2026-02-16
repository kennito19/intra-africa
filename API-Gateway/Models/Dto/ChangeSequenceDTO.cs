namespace API_Gateway.Models.Dto
{
    public class ChangeSequenceDTO
    {
        public int Id { get; set; }
        public int Sequence { get; set; }
        public List<ChangeChildSequenceDTO>? ChildSequence { get; set; }
    }

    public class ChangeChildSequenceDTO
    {
        public int Id { get; set; }
        public int ParentId { get; set; }
        public int Sequence { get; set; }
    }
}
