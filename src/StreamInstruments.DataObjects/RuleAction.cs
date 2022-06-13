using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StreamInstruments.DataObjects;

public class RuleAction : EntityBase
{
    /// <summary>
    /// The type of action to perform when the parent <see cref="Rule"/> gets activated.
    /// </summary>
    public ActionType Type { get; set; }

    /// <summary>
    /// Data required for this action to execute.
    /// </summary>
    public string Data { get; set; }

    /// <summary>
    /// The point at which this action should be performed when the parent <see cref="Rule"/>
    /// gets activated.
    /// </summary>
    /// <remarks>
    /// The first action is marked as 0, and increments with the number of sibling actions
    /// belonging to the parent <see cref="Rule"/>.
    /// </remarks>
    [Range(0, int.MaxValue, ErrorMessage = "Order must be greater than or equal to 0")]
    public int Order { get; set; }

    /// <summary>
    /// The rule entity this action belongs to.
    /// </summary>
    [ForeignKey("Rule_Id")]
    public virtual Rule Rule { get; set; }
}