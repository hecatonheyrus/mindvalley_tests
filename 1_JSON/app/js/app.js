window.ResumeViewer = {
  Models: {},
  Collections: {},
  Views: {},

  start: function(data) {

    var items = new ResumeViewer.Collections.Items(data.items),
        router = new ResumeViewer.Router();
   
    router.on('route:home', function() {
      router.navigate('items', {
        trigger: true,
        replace: true
      });
    });

    router.on('route:showItems', function() {

      var itemsView = new ResumeViewer.Views.Items({
        collection: items
      });
      
      $('.main-container').html(itemsView.render().$el);

    });


    Backbone.history.start();
  }
};
