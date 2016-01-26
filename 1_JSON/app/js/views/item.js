ResumeViewer.Views.Item = Backbone.View.extend({
  tagName: 'li',
  className: 'media col-md-6 col-lg-4',
  template: _.template($('#tpl-item').html()),

  events: {

  },

  initialize: function() {
    //this.listenTo(this.model, 'remove', this.remove);
  },

  render: function() {
    var html = this.template(this.model.toJSON());
    this.$el.append(html);
    return this;
  }

 
}); 